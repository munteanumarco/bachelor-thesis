FROM python:3.9-slim

WORKDIR /app

# Install dependencies
RUN apt-get update && apt-get install -y \
    build-essential \
    cmake \
    git \
    wget \
    unzip \
    yasm \
    pkg-config \
    libswscale-dev \
    libtbbmalloc2 \
    libtbb-dev \
    libjpeg-dev \
    libpng-dev \
    libtiff-dev \
    libavformat-dev \
    libpq-dev && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Install OpenCV
RUN pip install opencv-python-headless

COPY . /app

RUN pip install --no-cache-dir -r requirements.txt

EXPOSE 13500

ENV PORT 13500
ENV HOST 0.0.0.0

CMD ["python", "src/main.py"]