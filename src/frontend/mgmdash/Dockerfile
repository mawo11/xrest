FROM    mhart/alpine-node

RUN     npm install -g http-server

WORKDIR /site
ADD     ./site   /site

EXPOSE  8081

CMD ["http-server", "--cors", "-p8081", "/site"]