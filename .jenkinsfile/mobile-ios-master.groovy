pipeline {
    agent {
        node {
            // allow different job to share same workspace
            customWorkspace 'workspace/booster-math-master-ios'

            // select agent node with `unity` label
            label 'unity && xcode'
        }
    }
    environment {
        OUTDIR = 'output'
        TEMP_DIR = 'tmp'
        XCODE_PROJECT = 'Unity-iPhone.xcodeproj'
    }
    stages {
        stage('Build') {
            steps {
                mattermostStatus(null, 'Started.', '#CCCCCC', 'https://internal-jenkins.s3-ap-southeast-1.amazonaws.com/public/jenkins.png', null)
                s3BuildNumber('booster-math-ios')

                sh "rm -rf \"$OUTDIR\""
                sh "rm -rf \"$TEMP_DIR\""

                lock('unity') {
                    // source: https://bitbucket.org/hotplay-bitbucket/jenkins/src/master/vars/buildUnity.groovy
                    buildUnity(
                        projectPath: null,
                        outDir: "$OUTDIR",
                        unityHub: "$UNITY_HUB",
                        unityVersion: '2021.3.9f1',
                        target: 'iOS',
                        development: false,
                        obb: false,
                        symbols: 'TextMeshPro;AADOTWEEN;MOBILE_DEVICE;INPUT_SYSTEM_PACKAGE;UNITASK_DOTWEEN_SUPPORT;ODIN_INSPECTOR;ODIN_INSPECTOR_3;ODIN_INSPECTOR;ODIN_INSPECTOR_3;MOREMOUNTAINS_FEEDBACKS;MOREMOUNTAINS_TOOLS;MOREMOUNTAINS_TOOLS_FOR_MMFEEDBACKS;MOREMOUNTAINS_TEXTMESHPRO_INSTALLED;MOREMOUNTAINS_TEXTMESHPRO_INSTALLED;MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED;MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED',
                        compression: 'lz4hc',
                        buildInfoPath: 'BuildInfo/BuildInfo.json'
                    )
                }

                sh "mkdir -p \"$TEMP_DIR\""
                sh "mv \"$BUILD_PATH\" \"$TEMP_DIR/build\""
            }
        }

        stage('XCode') {
            steps {
                // unlock keychain
                withCredentials([string(credentialsId: "$NODE_NAME-keychain", variable: 'KEYCHAIN_PASS')]) {
                    sh """
                        security unlock-keychain -p \$KEYCHAIN_PASS login.keychain
                        security set-keychain-settings login.keychain
                    """
                }

                // fetch provisional profile
                withCredentials([file(credentialsId: 'fastlane-apple-api', variable: 'FASTLANE_API_KEY')]) {
                    sh """
                        bundle install
                        bundle exec fastlane sigh\
                            --api_key_path \$FASTLANE_API_KEY\
                            --readonly
                    """
                }

                // build
                lock('xcode') {
                    sh """
                        bundle exec fastlane gym\
                            --project "$WORKSPACE/$TEMP_DIR/build/${XCODE_PROJECT}"\
                            --build_path "$WORKSPACE/$TEMP_DIR"\
                            --output_directory "$WORKSPACE/$OUTDIR"\
                            --configuration "Release"\
                            --export_options "$WORKSPACE/.jenkinsfile/exportOptions.plist"\
                            --clean
                    """
                }

                sh "rm -rf \"$TEMP_DIR\""
            }
        }
    }
    post {
        always {
            retry(3) {
                archiveArtifacts artifacts: "$OUTDIR/*"
            }
            parseLog()
        }

        success {
            mattermostStatus(null, 'Success.', 'good', 'https://internal-jenkins.s3-ap-southeast-1.amazonaws.com/public/jenkins.png', null)
            deployFastlane('beta', 'jenkins', 'xcode')
        }

        unsuccessful {
            mattermostStatus(null, 'Failed.', '#FF6347', 'https://internal-jenkins.s3-ap-southeast-1.amazonaws.com/public/jenkins-failed.png', null)
        }
    }
}
