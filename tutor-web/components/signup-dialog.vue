<template>
  <div class>
    <v-container @click="toggleDialog">
      Sign Up <v-icon>mdi-account</v-icon>
    </v-container>

    <v-dialog v-model="dialog" width="450">
      <v-card>
        <v-container>
          <v-form ref="form" lazy-validation>
            <v-row>
              <v-col>
                <v-text-field
                  v-model="username"
                  ma="4"
                  label="Email"
                  required
                ></v-text-field>
              </v-col>
            </v-row>

            <v-row>
              <v-col>
                <v-text-field
                  v-model="password"
                  :append-icon="show ? 'mdi-eye' : 'mdi-eye-off'"
                  :type="show ? 'text' : 'password'"
                  label="Password"
                  counter
                  required
                  @click:append="show = !show"
                ></v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-text-field
                  v-model="firstname"
                  ma="4"
                  label="First Name"
                  required
                ></v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-text-field
                  v-model="lastname"
                  ma="4"
                  label="Last Name"
                  required
                ></v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col class="text-right">
                <v-btn color="primary" @click="submitCredentials()">
                  submit
                </v-btn>
              </v-col>
            </v-row>
          </v-form>
        </v-container>
      </v-card>
    </v-dialog>
  </div>
</template>
<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import { JWT } from '~/scripts/jwt'
@Component({})
export default class SettingsDialog extends Vue {
  dialog = false
  show = false
  username = ''
  password = ''
  firstname = ''
  lastname = ''
  toggleDialog() {
    this.dialog = !this.dialog
  }

  submitCredentials() {
    this.$axios
      .post('Token/RegisterStudent', {}, {
        params: {
          username: this.username,
          password: this.password,
          firstname: this.firstname,
          lastname: this.lastname,
        },
      })
      .then((result) => {
        console.log(result)
      })
  }
}
</script>
