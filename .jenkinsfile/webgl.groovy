pipeline {
    agent {
        node {
            // allow different job to share same workspace
            customWorkspace 'workspace/boooster-math-dev-webgl'

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
                statusInProgress('jenkins')
                s3BuildNumber('booster-math-webgl')

                lock('unity') {
                    // source: https://bitbucket.org/hotplay-bitbucket/jenkins/src/master/vars/buildUnity.groovy
                    buildUnity(
                        projectPath: null,
                        outDir: "$OUTDIR",
                        unityHub: "$UNITY_HUB",
                        unityVersion: '2021.3.9f1',
                        target: 'WebGL',
                        development: false,
                        symbols: 'DEBUG_BUILD;DEBUG;CHEAT_ENABLED;MOBILE_DEVICE;TextMeshPro;INPUT_SYSTEM_PACKAGE;UNITASK_DOTWEEN_SUPPORT;ODIN_INSPECTOR;ODIN_INSPECTOR_3;ODIN_INSPECTOR;ODIN_INSPECTOR_3;MOREMOUNTAINS_FEEDBACKS;MOREMOUNTAINS_TOOLS;MOREMOUNTAINS_TOOLS_FOR_MMFEEDBACKS;MOREMOUNTAINS_TEXTMESHPRO_INSTALLED;MOREMOUNTAINS_TEXTMESHPRO_INSTALLED;MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED;MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED',
                        compression: 'lz4hc',
                        buildInfoPath: 'BuildInfo/BuildInfo.json',
                        webTemplate: 'PROJECT:Ambiens'
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
            statusSuccess('jenkins')
            build job: 'deploy-static-webgl', parameters: [string(name: 'projectName', value: 'booster-math-webgl'), string(name: 'displayName', value: '(PG-05) Booster Math (Develop)')], wait: false
        }

        unsuccessful {
            statusFail('jenkins')
        }
    }
}
