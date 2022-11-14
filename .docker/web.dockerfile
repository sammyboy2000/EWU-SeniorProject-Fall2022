# We don't want to start from scratch.
# That is why we tell node here to use the current node image as base.
FROM node:16

# Create an application directory
RUN mkdir -p /web

# The /web directory should act as the main application directory
WORKDIR /web

# Copy the app package and package-lock.json file
COPY tutor-web/package*.json ./

# Install node packages
RUN npm install

# Copy or project directory (locally) in the current directory of our docker image (/app)
COPY tutor-web/ .

# Build the app
RUN npm run build

# Expose $PORT on container.
# We use a variable here as the port is something that can differ on the environment.
EXPOSE $PORT

# Set host to localhost / the docker image
ENV NUXT_HOST=0.0.0.0

# Set app port
ENV NUXT_PORT=$PORT

# Set the base url
ENV PROXY_API=$PROXY_API

# Set the browser base url
ENV PROXY_LOGIN=$PROXY_LOGIN

# Start the app
CMD [ "npm", "start" ]