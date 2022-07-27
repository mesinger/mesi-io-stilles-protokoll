# deploy
deploy-clean:
	rm -f image.tar && rm -f stilles-protokol.tar.gz

docker-build:
	docker build -t mesi/stilles-protokol --platform=x86_64 . && \
    docker save -o image.tar mesi/stilles-protokol
    
prepare-package:
	tar -czf stilles-protokol.tar.gz image.tar

ansible-deploy-subst:
	source ~/src/infra/sources/silent_protocol && \
	envsubst < deploy-staging.yml > tmp.yml && \
	cat tmp.yml && rm -f tmp.yml
    
ansible-deploy:
	source ~/src/infra/sources/silent_protocol && \
	envsubst < deploy-staging.yml > deploy-staging.subst.yml && \
	ansible-playbook deploy-staging.subst.yml && \
	rm -f deploy-staging.subst.yml

deploy: deploy-clean docker-build prepare-package ansible-deploy deploy-clean

# local
dev:
	docker compose up -d

stop:
	docker compose down
