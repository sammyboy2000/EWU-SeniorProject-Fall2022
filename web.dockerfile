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

#Generate the static portion with Nuxt
RUN npm run generate

# Expose port on container.
# We use 3000 as it's the default Nuxt app port.
EXPOSE 3000

# Set host to localhost / the docker image
ENV NUXT_HOST=0.0.0.0

# Set app port
ENV NUXT_PORT=3000

# Set the base url
ENV PROXY_API=$PROXY_API

# Set the browser base url
ENV PROXY_LOGIN=$PROXY_LOGIN

# Start the app
CMD [ "npm", "start" ]