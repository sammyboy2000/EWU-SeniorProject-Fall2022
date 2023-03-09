import { NuxtAxiosInstance } from '@nuxtjs/axios'

class TutorToken {
  Random: string = ''
  UserId: string = ''
  UserName: string = ''
  aud: string = ''
  exp: number = 0
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role': string[] = []
  iss: string = ''
  jti: string = ''
  sub: string = ''
  get roles(): string[] {
    return this['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
  }
}
export class JWT {
  private static tokenInstance: string
  private static _tokenData: TutorToken

  public static setToken(token: string, axios: NuxtAxiosInstance) {
    this.tokenInstance = token
    axios.setHeader('Authorization', `Bearer ${token}`)
    const parts = token.split('.')
    const payload = JSON.parse(atob(parts[1]))
    this._tokenData = Object.assign(new TutorToken(), payload)
    this.storeToken()
  }

  // Added by Jesse: 2/28/2023
  public static getUserName(): string {
    return this.tokenData.sub
  }

  public static getToken(): string {
    return this.tokenInstance
  }

  public static get tokenData(): TutorToken {
    return this._tokenData
  }

  public static storeToken(): void {
    localStorage.clear()
    const token = this.tokenInstance
    localStorage.setItem('TutorToken', JSON.stringify(token))
  }

  public static loadToken(axios: NuxtAxiosInstance): string {

    const storageToken = localStorage.getItem('TutorToken')

    if (storageToken !== null) {
      this.setToken(storageToken, axios)
    }
    return this.tokenInstance
  }

  public static deleteToken(axios: NuxtAxiosInstance): void {
    localStorage.removeItem('TutorToken')
    this.tokenInstance=null!
    location.reload()
  }
}
