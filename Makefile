# deploy
sql-scripts:
	rm -rf sql && \
	dotnet ef migrations script 0 InitialCreate -p src/mesi-io-silent-protocol-infra-db -s src/mesi-io-silent-protocol-webapp -o sql/01_initial_create.sql && \
	dotnet ef migrations script InitialCreate AddCreatedAtRow -p src/mesi-io-silent-protocol-infra-db -s src/mesi-io-silent-protocol-webapp -o sql/02_add_created_at_row.sql

deploy-clean:
	rm -f image.tar -f stilles-protokol.tar.gz && rm -rf sql/

docker-build:
	docker build -t mesi/stilles-protokol --platform=x86_64 . && \
    docker save -o image.tar mesi/stilles-protokol
    
prepare-package:
	tar -czf stilles-protokol.tar.gz image.tar sql/

ansible-deploy-subst:
	source ~/src/infra/sources/silent_protocol && \
	envsubst < deploy-staging.yml > tmp.yml && \
	cat tmp.yml && rm -f tmp.yml
    
ansible-deploy:
	source ~/src/infra/sources/silent_protocol && \
	envsubst < deploy-staging.yml > deploy-staging.subst.yml && \
	ansible-playbook deploy-staging.subst.yml && \
	rm -f deploy-staging.subst.yml

deploy: deploy-clean sql-scripts docker-build prepare-package ansible-deploy deploy-clean

# local
db:
	docker compose up -d postgres

start-infra: db apply-local-migrations

stop-infra:
	docker compose down

apply-local-migrations:
	dotnet ef database update -p src/mesi-io-silent-protocol-infra-db -s src/mesi-io-silent-protocol-webapp
