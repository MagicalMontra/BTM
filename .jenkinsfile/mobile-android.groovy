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
               // statusInProgress('jenkins')
                statusinprogressgitlab('jenkins')
                s3BuildNumber('booster-math-android')

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
                        symbols: 'DEBUG;DEBUG_BUILD;CHEAT_ENABLED;TextMeshPro;AADOTWEEN;MOBILE_DEVICE;INPUT_SYSTEM_PACKAGE;UNITASK_DOTWEEN_SUPPORT;ODIN_INSPECTOR;ODIN_INSPECTOR_3;ODIN_INSPECTOR;ODIN_INSPECTOR_3;MOREMOUNTAINS_FEEDBACKS;MOREMOUNTAINS_TOOLS;MOREMOUNTAINS_TOOLS_FOR_MMFEEDBACKS;MOREMOUNTAINS_TEXTMESHPRO_INSTALLED;MOREMOUNTAINS_TEXTMESHPRO_INSTALLED;MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED;MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED',
                        compression: 'lz4hc',
                        buildInfoPath: 'BuildInfo/BuildInfo.json'
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
          //  statusSuccess('jenkins')
            statusSuccessGitLab('jenkins')
            deployFastlane('firebase', 'jenkins')
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
