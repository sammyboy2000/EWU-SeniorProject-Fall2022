import { NuxtAxiosInstance } from '@nuxtjs/axios'

export async function AuthenticationCheck($axios: NuxtAxiosInstance) {
  let permLevel = -1
  await $axios
    .get('Token/AuthCheck')
    .then((result) => {
      if (result.data === 'Authorized as Student') permLevel = 0
      else if(result.data === 'Authorized as Tutor') permLevel = 1
      else if(result.data === 'Authorized as Admin') permLevel = 2
      else permLevel = -1
    })
    .catch(function (error) {
        console.log('Unauthorized Access' + error)
    })
  return permLevel
}