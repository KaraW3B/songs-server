FROM mcr.microsoft.com/dotnet/sdk:10.0

EXPOSE 7373

RUN apt-get update
RUN apt-get install -y curl git libpng-dev libjpeg-dev curl libxi6 build-essential libgl1-mesa-glx
RUN curl -sL https://deb.nodesource.com/setup_lts.x | bash -
RUN apt-get install -y nodejs

ARG UUID=1001
ARG GUID=1001
RUN groupadd -g $GUID -o karaweb
RUN useradd -m -u $UUID -g $GUID -o -s /bin/bash karaweb

RUN mkdir /app
RUN chown karaweb:karaweb /app

USER karaweb:karaweb

RUN mkdir /home/karaweb/src

RUN mkdir /home/karaweb/src/Resources
WORKDIR /home/karaweb/src/Resources
COPY Resources/* .

RUN mkdir /home/karaweb/src/Host
WORKDIR /home/karaweb/src/Host
COPY Host/* .
RUN dotnet build --configuration Release

RUN mkdir /home/karaweb/src/Front
WORKDIR /home/karaweb/src/Front
COPY Front/* .
RUN npm install
RUN npm run build

WORKDIR /app
RUN mv /home/mediasaccess/src/bin/Release/* ./

RUN rm -r /home/karaweb/src

ENTRYPOINT ["dotnet", "run", "KaraWeb.exe"]