#HOWTO GIT

###Create a new repository on the command line
    
    touch README.md
    git init
    git add README.md
    git commit -m "first commit"
    git remote add origin https://github.com/<username>/next_app.git
    git remote add origin https://github.com/wouldyougo/next_app.git
    git push -u origin master

###Push an existing repository from the command line
    
    git remote add origin https://github.com/<username>/next_app.git
    git remote add origin https://github.com/wouldyougo/next_app.git
    git push -u origin master

### Настройка репозиория GIT
    
    git config --global user.email "user_email@gmail.com"
    git config --global user.name "wouldyougo"
    git config --global --list

### git init
    
    git init

### git commit
    
    git add .
    git commit -m "Initial commit"
    git mv README.rdoc README.md
    git commit -am "Improve the README"
    ...
    git commit -am "Add files"

### git push
Так вы можете отправить изменения на GitHub:
    
    git remote add origin https://github.com/wouldyougo/next_app.git
    git push -u origin master


### Работа в отдельной ветке
При использовании Git, хорошей практикой является делать изменения в отдельной ветке,
а не в master ветке. Перешел к новой ветке 'static-pages':
    
    git checkout -b static-pages
    git add .
    git commit -m "Finish static pages"

Затем объедините изменения с мастер веткой:
    
    git checkout master
    git merge static-pages

Затем  вы можете отправить изменения на GitHub и heroku:
    
    git push
    git push heroku

## heroku
### Регистрация на Heroku
Первый шаг это регистрация на Heroku;
после подтверждения вашего email для завершения создания вашего аккаунта,
установите необходимый для Heroku софт с помощью Heroku Toolbelt:
    
    wget -qO- https://toolbelt.heroku.com/install-ubuntu.sh | sh

    $ heroku login
    Enter your Heroku credentials.
    Email: wouldyougo@gmail.com
    Password:
    Could not find an existing public key.
    Would you like to generate one? [Yn]
    Generating new SSH public key.
    Uploading ssh public key /Users/adam/.ssh/id_rsa.pub

### Развертывание на Heroku
    
    $ cd ~/myapp
    heroku create
    git push heroku master
    heroku run rake db:migrate

Так вы можете развернуть приложение на сервере Heroku:
    
    git push heroku
    heroku pg:reset DATABASE
    heroku pg:reset DATABASE --confirm fierce-waters-1951
    heroku run rake db:migrate
    heroku run rake db:populate
    heroku run rake db:postulate
    heroku run rake db:relations

### heroku restart
Для того чтобы увидеть изменения вам возможно придется принудительно рестартовать приложение на Heroku:
    
    heroku restart
    heroku open

Если столкнулись с проблемами, попробуйте выполнить
    
    heroku logs

### clone git@heroku

    git clone git@heroku.com:fierce-waters-1951.git
    git remote remove origin
    git remote add heroku git@heroku.com:fierce-waters-1951.git

###Пример использования git и heroku
    
    git checkout master
    git checkout -b static-pages
    git add .
    git commit -am "Finish static pages"
    git checkout master
    git merge static-pages
    git push
    git push heroku

    heroku pg:reset DATABASE
    heroku pg:reset DATABASE --confirm fierce-waters-1951
    heroku run rake db:migrate
    heroku run rake db:populate
    heroku run rake db:postulate
    heroku run rake db:relations
    heroku restart
    heroku open

    heroku pg:reset DATABASE --confirm fierce-waters-1951
    heroku run rake db:migrate
    heroku run rake db:populate
    heroku run rake db:user_orders
    heroku restart
    heroku open

### git stash
    
    git stash

### git pull

    git clone /var/repos/gap_app.git
    git pull /var/repos/gap_app.git


git remote add origin https://wouldyougo@bitbucket.org/wouldyougo/trs.git
git remote add github https://github.com/wouldyougo/TRx.git

