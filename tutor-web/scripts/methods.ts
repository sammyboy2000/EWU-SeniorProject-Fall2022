import { NuxtAxiosInstance } from '@nuxtjs/axios'
import { Question, AnsweredQuestion } from '~/scripts/interfaces'

export async function AuthenticationCheck($axios: NuxtAxiosInstance) {
  let permLevel = [-1, -1, -1]
  await $axios
    .get('Token/AuthCheck')
    .then((result) => {
      if (result.data[0] === 'Authorized as Student') permLevel[0] = 0
      if (result.data[1] === 'Authorized as Tutor') permLevel[1] = 1
      if (result.data[2] === 'Authorized as Admin') permLevel[2] = 2
      console.log("Results: ", result.data)
    })
    .catch(function (error) {
      console.log('Unauthorized Access' + error)
    })
  return permLevel
}

export async function getAnsweredQuestionTopicName(
  questionList: AnsweredQuestion[],
  $axios: NuxtAxiosInstance
) {
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

export async function getAnsweredQuestionClassCode(
  questionList: AnsweredQuestion[],
  $axios: NuxtAxiosInstance
) {
  const classCodeList: string[] = []
  for (let j = 0; j < questionList.length; j++) {
    await $axios
      .get('/database/getClassCode', {
        params: {
          i: questionList[j].classId,
        },
      })
      .then((response) => {
        classCodeList[j] = response.data
      })
  }
  return classCodeList
}

export async function getAskedQuestionTopicName(
  questionList: Question[],
  $axios: NuxtAxiosInstance
) {
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
