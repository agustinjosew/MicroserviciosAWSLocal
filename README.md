# Microservicios con AWS de manera local
 Entorno de microservicios local que imita un entorno de producción en AWS, usando tecnologías como AWS Lambda, API Gateway, SNS, SQS, S3, DocumentDB, y monitoreo con AWS X-Ray. para demostrar cómo se pueden desplegar y monitorear microservicios en AWS

## Objetivo de la PoC

Desarrollar un entorno local de microservicios utilizando Docker que simule una arquitectura de producción en AWS, incorporando prácticas de seguridad con SAML y gestión de infraestructura con Terraform.

### Tecnologías a Utilizar

Docker y Docker Compose: Para contenerizar los microservicios y sus dependencias.
AWS CLI: Para interactuar con AWS desde nuestro entorno local.
Terraform: Para definir la infraestructura como código, lo que permite una fácil replicación del entorno en la nube.
MinIO: Como alternativa local a AWS S3 para el almacenamiento de objetos.
MongoDB: Como alternativa local a DocumentDB para la base de datos.
SAML: Para la autenticación y autorización, posiblemente usando una implementación local de pruebas como saml2aws o implementando un Identity Provider (IdP) simulado.
AWS X-Ray Daemon: Ejecutándose en un contenedor para recopilar trazas de los microservicios para el análisis.
