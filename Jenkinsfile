
pipeline {
    agent any

    stages {
        stage('Checkout') {
            steps {
                echo 'Repository checkout successful'
            }
        }

        stage('Find Calculator') {
            steps {
                sh 'find . -name "*.csproj" -not -path "*/obj/*" -not -path "*/bin/*"'
            }
        }

        stage('Build Calculator') {
            steps {
                sh '''
                    PROJECT=$(find . -name "*.csproj" -not -path "*/obj/*" -not -path "*/bin/*" | head -n 1)
                    if [ -z "$PROJECT" ]; then
                        echo "ERROR: .csproj not found"
                        exit 1
                    fi
                    echo "Building $PROJECT"
                    dotnet build "$PROJECT" --configuration Release
                '''
            }
        }
    }
}
