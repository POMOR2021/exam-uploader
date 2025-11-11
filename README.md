# ImageUploaderApp

## Быстрый старт

### Локально
```bash
# Создать PostgreSQL базу и поправить строку подключения в appsettings.json (или через переменные)
dotnet run
```

### Контейнер
```bash
docker build -t image-uploader .
docker run -e S3__BucketName=your-bucket -e S3__AccessKey=your-key -e S3__SecretKey=your-secret -e S3__ServiceUrl=https://storage.yandexcloud.net -e S3__Region=ru-central1 -e ConnectionStrings__DefaultConnection="Host=host;Port=5432;Database=db;Username=user;Password=pass" -p 8080:8080 image-uploader
```

### Описание
- Метаданные картинок — в PostgreSQL (EF+auto migrate).
- Картинки — в Yandex Object Storage (через S3 API).
- Все ключи — через переменные окружения.

## Следующие шаги: Kubernetes, CI/CD, Yandex Cloud.

## Kubernetes (Яндекс MSK)

1. Заполнить секреты (S3, PG, Container Registry) в k8s-deployment.yaml.
2. Применить:
```bash
kubectl apply -f k8s-deployment.yaml
```

## CI/CD (GitHub Actions)
- Репозиторий должен содержать secrets: YC_CR_TOKEN, YC_CR_REGISTRY, YC_CR_REPO, KUBE_CONFIG.
- Пуш в main запустит сборку, загрузку Docker-образа и автоматический деплой.
