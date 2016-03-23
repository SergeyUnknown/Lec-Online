# About the project

## Prerequiresites

* SQL Server
* IIS

## Dev prerequiresites

* VS 2013
* Node.js 0.10
* Configuration Section Designer 2.0 (https://csd.codeplex.com/releases).
  It is used for the generation of .NET configuration sections.

Additional steps

```
npm install -g lessc
```

### Build Bootstrap theme

Normal production build

```
lessc src/LecOnline/Content/bootstrap/theme.less src/LecOnline/Content/bootstrap-theme.css
```

Debug build

```
lessc src/LecOnline/Content/bootstrap/theme.less src/LecOnline/Content/bootstrap-theme.css --source-map --source-map-less-inline
```
