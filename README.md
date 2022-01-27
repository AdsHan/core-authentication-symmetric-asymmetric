# core-elasticsearch-nest
Demonstração de como podemos utilizar o Elasticsearch como ferramenta de busca em conjunto com o NEST para construção de queries.

# Como executar:
- Clonar / baixar o repositório em seu workplace.
- Baixar o .Net Core SDK e o Visual Studio / Code mais recentes.
- A infraestrutura da aplicação será criada através do docker compose /docker/docker_infrastructure.yml.
- Quando a aplicação for iniciada, será criado um índice com dados no Elasticsearch chamado **people**. Caso deseje fazer a carga manualmente, basta fazer um Post no endpoint `http://localhost:9200/people/_bulk?pretty` enviando o arquivo `people_NDJSON.json`

# Sobre
Este projeto foi desenvolvido por Anderson Hansen sob [MIT license](LICENSE).