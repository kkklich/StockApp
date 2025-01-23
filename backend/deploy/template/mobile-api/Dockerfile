FROM  mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app

RUN apt update
RUN apt install -y libgdiplus zip
RUN ln -s /usr/lib/libgdiplus.so /lib/x86_64-linux-gnu/libgdiplus.so
RUN apt-get install -y --no-install-recommends zlib1g fontconfig libfreetype6 libx11-6 libxext6 libxrender1 wget gdebi
RUN wget https://github.com/wkhtmltopdf/wkhtmltopdf/releases/download/0.12.5/wkhtmltox_0.12.5-1.stretch_amd64.deb
RUN gdebi --n wkhtmltox_0.12.5-1.stretch_amd64.deb
RUN apt install libssl1.1
RUN ln -s /usr/local/lib/libwkhtmltox.so /usr/lib/libwkhtmltox.so

#COPY ./PT_Sans.zip /usr/share/fonts/truetype/pt_sans/PT_Sans.zip

#RUN mkdir -p /usr/share/fonts/truetype/pt_sans \
#    && unzip /usr/share/fonts/truetype/pt_sans/PT_Sans.zip -d /usr/share/fonts/truetype/pt_sans \
#    && fc-cache -f -v

COPY ./build/ .

EXPOSE 6231
ENTRYPOINT ["dotnet", "AF_mobile_web_api.dll"]
