# Comandos de git
* Refrescar en local ramas remotas:
```bash
git remote update origin --prune
```
* Ver ramas remotas y locales:
```bash
git branch -a
```
* Cambiar a nueva rama local y con la propiedad -b se crea si no existe
```bash
git checkout NOMBRE_RAMA
git checkout -b NOMBRE_RAMA
```
* Actualizar y sobreescribir rama local con remota
```bash
git reset --hard origin/NOMBRE_RAMA
```
* Modificar URL de repositorio remoto
```bash
git remote set-url origin NUEVA_URL
```

# Comandos de docker
* Descargar una imagen
```bash
# Sintaxis
docker pull NOMBRE_IMAGEN[:VERSION]
# Ejemplo
docker pull nestjs/cli:8.0.0
```
* Ejecutar image
```bash
docker run -it -rm -p 3000:3000 -v $(pwd)/workspace nestjs/cli[:VERSION]
```