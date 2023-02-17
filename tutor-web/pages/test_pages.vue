<template>
    <container fluid>
        <v-row>
            <v-col>
                <v-title>
                    This page is used for testing purposes and contains
                    a directory to test pages
                </v-title>
                <v-text-card>
                    <br />
                    <v-btn color="primary" nuxt to="/testLogin">
                        Test Login
                    </v-btn>
                    <br /><br />
                    <v-btn color="primary" nuxt to="/tutor_test">
                        Test Tutor
                    </v-btn>
                    <br /><br />
                    <v-btn color="primary" nuxt to="/test-sql">
                        Test SQL
                    </v-btn>
                    <br /><br />
                    <v-btn color="primary" nuxt to="/test-sqladdclass">
                        Test SQL Add Class
                    </v-btn>
                </v-text-card>
            </v-col>
        </v-row>
    </container>
</template>