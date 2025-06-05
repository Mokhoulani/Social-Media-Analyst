import React, { useState } from 'react'
import { ActivityIndicator, Button, Text, TextInput, View } from 'react-native'
import { useSelector } from 'react-redux'
import { z } from 'zod'
import { AuthFacade } from '../store/auth/facade'
import { authSelectors } from '../store/auth/selectors'
import { useStyles } from '../themes/useStyles'

const loginSchema = z.object({
    email: z.string().email('Invalid email address'),
    password: z.string().min(6, 'Password must be at least 6 characters'),
})

type LoginInput = z.infer<typeof loginSchema>

export function LoginScreen() {
    const [form, setForm] = useState<LoginInput>({
        email: '',
        password: '',
    })
    const styles = useStyles()

    const [errors, setErrors] = useState<Partial<LoginInput>>({})

    const isLoading = useSelector(authSelectors.selectLoading)
    const error = useSelector(authSelectors.selectError)
    const isAuthenticated = useSelector(authSelectors.selectIsAuthenticated)

    const handleChange = (key: keyof LoginInput, value: string) => {
        setForm((prev) => ({ ...prev, [key]: value }))
        setErrors((prev) => ({ ...prev, [key]: undefined })) // clear error on input
    }

    const handleLogin = () => {
        const result = loginSchema.safeParse(form)
        if (!result.success) {
            const zodErrors = result.error.flatten().fieldErrors
            setErrors({
                email: zodErrors.email?.[0],
                password: zodErrors.password?.[0],
            })
            return
        }

        AuthFacade.login(form.email, form.password)
    }

    return (
        <View style={styles.container}>
            <Text style={styles.title}>Login Test</Text>

            <TextInput
                style={styles.input}
                placeholder="Email"
                onChangeText={(text) => handleChange('email', text)}
                value={form.email}
                autoCapitalize="none"
                keyboardType="email-address"
            />
            {errors.email && <Text style={styles.error}>{errors.email}</Text>}

            <TextInput
                style={styles.input}
                placeholder="Password"
                onChangeText={(text) => handleChange('password', text)}
                value={form.password}
                secureTextEntry
            />
            {errors.password && (
                <Text style={styles.error}>{errors.password}</Text>
            )}

            <Button title="Login" onPress={handleLogin} />
            {isLoading && <ActivityIndicator style={{ marginTop: 10 }} />}
            {isAuthenticated && (
                <Text style={styles.success}>Authenticated ✅</Text>
            )}
            {error && <Text style={styles.error}>{error}</Text>}
        </View>
    )
}
