pipeline {
    agent {
        node {
            // allow different job to share same workspace
            customWorkspace 'workspace/booster-math-dev-android'

            // select agent node with `unity` label
            label 'unity'
        }
    }
    environment {
        OUTDIR='output'
    }
    stages {
        stage('Build') {
            steps {
              //  statusInProgress('jenkins')
                statusinprogressgitlab('jenkins')
                s3BuildNumber('booster-math-android-tv')

                lock('unity') {
                    // source: https://bitbucket.org/hotplay-bitbucket/jenkins/src/master/vars/buildUnity.groovy
                    buildUnity(
                        projectPath: null,
                        outDir: "$OUTDIR",
                        unityHub: "$UNITY_HUB",
                        unityVersion: '2021.3.9f1',
                        target: 'Android',
                        development: false,
                        obb: false,
                        symbols: 'TV_DEVICE;NAVIGATION;AADOTWEEN;DEBUG_BUILD;INPUT_SYSTEM_PACKAGE;CHEAT_ENABLED;TextMeshPro',
                        compression: 'lz4hc',
                        buildInfoPath: 'BuildInfo/BuildInfo.json',
                        applicationIdentifier: 'com.hotplay.boostermath.tv',
                        defaultInterfaceOrientation: 'LandscapeLeft'
                    )
                }
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
           // statusSuccess('jenkins')
            statusSuccessGitLab('jenkins')
        }

        unsuccessful {
           // statusFail('jenkins')
           statusFailGitlab('jenkins')
        }
    }
    options {
      gitLabConnection('Gitlab-Hotplay-Games')
    }

}
