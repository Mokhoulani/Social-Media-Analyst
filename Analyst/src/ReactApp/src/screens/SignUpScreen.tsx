import React, { useState } from 'react'
import {
    ActivityIndicator,
    Button,
    StyleSheet,
    Text,
    TextInput,
    View,
} from 'react-native'
import { useSelector } from 'react-redux'
import { AuthFacade, SignUpForm, signUpSchema } from '../store/auth/facade'
import { authSelectors } from '../store/auth/selectors'

export function SignUpScreen() {
    const [form, setForm] = useState<SignUpForm>({
        firstName: '',
        lastName: '',
        email: '',
        password: '',
    })
    const [errors, setErrors] = useState<
        Partial<Record<keyof SignUpForm, string>>
    >({})

    const isLoading = useSelector(authSelectors.selectLoading)
    const error = useSelector(authSelectors.selectError)
    const isAuthenticated = useSelector(authSelectors.selectIsAuthenticated)

    const handleChange = (field: keyof SignUpForm, value: string) => {
        setForm({ ...form, [field]: value })
    }

    const handleSignUp = () => {
        const result = signUpSchema.safeParse(form)
        if (!result.success) {
            const zodErrors = result.error.flatten().fieldErrors
            setErrors({
                firstName: zodErrors.firstName?.[0],
                lastName: zodErrors.lastName?.[0],
                email: zodErrors.email?.[0],
                password: zodErrors.password?.[0],
            })
            return
        }

        setErrors({})
        AuthFacade.signUp(form)
    }

    return (
        <View style={styles.container}>
            <Text style={styles.title}>Sign Up</Text>

            <TextInput
                style={styles.input}
                placeholder="First Name"
                value={form.firstName}
                onChangeText={(text) => handleChange('firstName', text)}
            />
            {errors.firstName && (
                <Text style={styles.error}>{errors.firstName}</Text>
            )}

            <TextInput
                style={styles.input}
                placeholder="Last Name"
                value={form.lastName}
                onChangeText={(text) => handleChange('lastName', text)}
            />
            {errors.lastName && (
                <Text style={styles.error}>{errors.lastName}</Text>
            )}

            <TextInput
                style={styles.input}
                placeholder="Email"
                autoCapitalize="none"
                value={form.email}
                onChangeText={(text) => handleChange('email', text)}
            />
            {errors.email && <Text style={styles.error}>{errors.email}</Text>}

            <TextInput
                style={styles.input}
                placeholder="Password"
                secureTextEntry
                value={form.password}
                onChangeText={(text) => handleChange('password', text)}
            />
            {errors.password && (
                <Text style={styles.error}>{errors.password}</Text>
            )}

            <Button title="Sign Up" onPress={handleSignUp} />

            {isLoading && <ActivityIndicator style={{ marginTop: 10 }} />}
            {isAuthenticated && (
                <Text style={styles.success}>Successfully registered ✅</Text>
            )}
            {error && <Text style={styles.error}>{error}</Text>}
        </View>
    )
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        padding: 16,
        backgroundColor: '#fff',
        justifyContent: 'center',
    },
    title: {
        fontSize: 24,
        fontWeight: 'bold',
        textAlign: 'center',
        marginBottom: 24,
    },
    input: {
        borderWidth: 1,
        borderColor: '#ccc',
        borderRadius: 8,
        padding: 12,
        marginBottom: 8,
    },
    success: {
        marginTop: 12,
        color: 'green',
        textAlign: 'center',
    },
    error: {
        marginTop: 4,
        marginBottom: 8,
        color: 'red',
        textAlign: 'left',
        marginLeft: 4,
    },
})
