import { ValidationPipe } from '@nestjs/common';
import { NestFactory } from '@nestjs/core';
import {
  DocumentBuilder,
  SwaggerModule,
} from '@nestjs/swagger';
import { AppModule } from './app.module';

async function bootstrap() {
  const app = await NestFactory.create(AppModule);
  
   app.useGlobalPipes(
    new ValidationPipe({
      whitelist: true,
      forbidNonWhitelisted: true,
      transform: true,
    }),
  );

  const swaggerConfig = new DocumentBuilder()
    .setTitle('Nexora Catalog Service')
    .setDescription(
      'Product catalog service for Nexora',
    )
    .setVersion('1.0')
    .build();

  const document =
    SwaggerModule.createDocument(
      app,
      swaggerConfig,
    );

  SwaggerModule.setup(
    'swagger',
    app,
    document,
  );

  const port =
    process.env.PORT ?? 3001;

  await app.listen(port);

  console.log(
    `Catalog Service running on http://localhost:${port}`,
  );
}
void bootstrap();
