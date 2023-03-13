docker tag portfolio20:latest registry.heroku.com/iaroportfolio/web
docker push registry.heroku.com/iaroportfolio/web
heroku container:release web --app=iaroportfolio
PAUSE