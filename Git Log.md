git checkout -b dev
git add .
git commit -m "Обновление структуры"

git branch -D dev

git checkout master
git merge dev

