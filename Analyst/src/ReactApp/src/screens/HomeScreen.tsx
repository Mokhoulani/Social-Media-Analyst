import { StatusBar } from 'expo-status-bar'
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
import { AuthFacade } from '../store/auth/facade'
import { authSelectors } from '../store/auth/selectors'
import { UserFacade } from '../store/user/facade'
import { userSelectors } from '../store/user/selectors'

export function HomeScreen() {
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')

    const isLoading = useSelector(authSelectors.selectLoading)
    const error = useSelector(authSelectors.selectError)
    const isAuthenticated = useSelector(authSelectors.selectIsAuthenticated)
    const isLoadingUser = useSelector(userSelectors.selectLoading)
    const user = useSelector(userSelectors.selectUser)
    const errorUser = useSelector(userSelectors.selectError)
    const isAuthenticatedUser = useSelector(userSelectors.selectIsAuthenticated)

    const handleLogin = () => {
        AuthFacade.login(email, password)
    }

    return (
        <View style={styles.container}>
            <Text style={styles.title}>Login Test</Text>
            <TextInput
                style={styles.input}
                placeholder="Email"
                onChangeText={setEmail}
                autoCapitalize="none"
            />
            <TextInput
                style={styles.input}
                placeholder="Password"
                onChangeText={setPassword}
                secureTextEntry
            />
            <Button title="Login" onPress={handleLogin} />
            {isLoading && <ActivityIndicator style={{ marginTop: 10 }} />}
            {isAuthenticated && (
                <Text style={styles.success}>Authenticated ✅</Text>
            )}
            {error && <Text style={styles.error}>{error}</Text>}
            <Button title="Get User" onPress={() => UserFacade.getUser()} />
            {isLoadingUser && <ActivityIndicator style={{ marginTop: 10 }} />}
            {isAuthenticatedUser && (
                <Text style={styles.success}>{user?.firstName}</Text>
            )}
            {errorUser && <Text style={styles.error}>{errorUser}</Text>}
            <StatusBar style="auto" />
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
        marginBottom: 12,
    },
    success: {
        marginTop: 12,
        color: 'green',
        textAlign: 'center',
    },
    error: {
        marginTop: 12,
        color: 'red',
        textAlign: 'center',
    },
})
