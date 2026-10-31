
pipeline {
    agent any

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Build Docker image') {
            steps {
                sh '''
                    docker build \
                      -t 7_calculator-web:latest \
                      -f App_practical/7_Calculator/Calculator/Dockerfile \
                      App_practical/7_Calculator/Calculator
                '''
            }
        }

        stage('Deploy Calculator') {
            steps {
                sh '''
                    docker rm -f calculator_olesya_5007 || true

                    docker run -d \
                      --name calculator_olesya_5007 \
                      --restart unless-stopped \
                      -p 5007:5007 \
                      7_calculator-web:latest
                '''
            }
        }
    }
}
