# 🩺 Turno Médico Backend

Este es el backend del sistema de gestión de turnos médicos desarrollado con **C# (.NET Core)** y **MongoDB**. Forma parte del proyecto final de la Tecnicatura en Desarrollo y Calidad de Software (UNSTA).

---

## ✅ Funcionalidades Implementadas

- Conexión con base de datos **MongoDB**
- Configuración centralizada en `appsettings.json`
- CRUD completo para **pacientes**:
  - Crear, listar, buscar por ID, editar y eliminar
- CRUD básico para **turnos médicos**:
  - Crear turno
  - Listar todos los turnos
  - Buscar turnos por paciente
  - Eliminar turno
- Documentación interactiva con **Swagger**

---

## 🛠️ Tecnologías

- ASP.NET Core 7
- MongoDB + MongoDB.Driver
- Swagger para pruebas de endpoints
- GitHub para control de versiones

---

## 🧪 Cómo correr el backend

```bash
dotnet build
dotnet run

Próximas tareas a implementar
 Agregar login y registro con encriptación de contraseñas

 Autenticación con JWT

 CRUD para profesionales

 Validación de disponibilidad de turnos

 Roles y permisos (paciente / profesional)

 Conexión con el frontend (React)

 Crear entorno de despliegue (Render / Railway)