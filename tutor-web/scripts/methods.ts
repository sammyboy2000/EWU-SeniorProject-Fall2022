import { NuxtAxiosInstance } from '@nuxtjs/axios'
import { Question, AnsweredQuestion } from '~/scripts/interfaces'

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

export async function getAnsweredQuestionTopicName(questionList: AnsweredQuestion[], $axios: NuxtAxiosInstance) {
    const topicList: string[] = []
    for (let j = 0; j < questionList.length; j++) {
        await $axios
        .get('/database/getTopicName', {
            params: {
            i: questionList[j].topicId,
            },
        })
        .then((response) => {
            topicList[j] = response.data
        })
    }
    return topicList
}

export async function getAskedQuestionTopicName(questionList: Question[], $axios: NuxtAxiosInstance) {
    const topicList: string[] = []
    for (let j = 0; j < questionList.length; j++) {
        await $axios
        .get('/database/getTopicName', {
            params: {
            i: questionList[j].topicId,
            },
        })
        .then((response) => {
            topicList[j] = response.data
        })
    }
    return topicList
}