# TodoApi

This is a simple Todo List API built in .NET 8. This project is currently being used for .NET full-stack candidates.

## Base de datos

The project comes with a devcontainer that provisions a SQL Server database. If you are not going to use the devcontainer, make sure to provision a SQL Server database and
update the connection string.

## Build

En mi caso se utilizó el devcontainer proporcionado para correr la aplicacion, dentro de la terminal generada correr los siguientes comandos:

```bash
# Compilar la API
dotnet build

# Crear/Actualizar base de datos
dotnet ef database update

# Correr la API
dotnet run

# o

dotnet run --project TodoApi
```

## Test

To run tests:

`dotnet test`

## Cambios Realizados

En esta seccion se documentan los cambios realizados al proyecto base que me fue proporcionado

Se crearon servicios:  
TodoListService   
TodoItemService

Antes el controller termina gestionando validaciones, por ejemplo si no existe el item que quieres actualizar en la lista. Además esto viola el principio SOLID. Single Responsibility, exponer la API y aplicar Lógica de Negocio. Entiendo que esto fue hecho así por una cuestion de la simplicidad del proyecto.

Ahora las dependencias quedan hacia abajo:

![Diagrama de paquetes](./documentacion/paquetes.png)

- Se mockea en los tests
- Dtos para no exponer directamente la entidad como respuesta de la Api	

Se agregó dependencia:
TodoApi -> Linq

## Nuevas Funcionalidades

![Diagrama de clases](./documentacion/clases.png)

Composicion entre la TodoList y los TodoItems. Si se elimina la lista se eliminan tambien los items asociados a ella.

Se exponen los siguientes nuevos endpoints:

Tabla

## Oportunidades de Mejora

En esta seccion se listan oportunidades de mejora identificadas, pero que por una cuestion de tiempo no se implementaron:

- No mostrar la id en las rutas
- Excepciones personalizadas
- Tests más exhaustivos, solo se probó el caso feliz


## Experiencia de Implementación

Ya habia ralizado apis en .net anteriormente, por lo que esa parte no fue tan complicada. Llevo su tiempo hacer andar el devcontainer, ya que nunca habia escuchado de esta herramienta. Fue de mucha utilidad para correr la aplicacion, y mas importante, para correr la base de datos en docker sin tener que crear ninguna base de datos por fuera.