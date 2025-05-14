import { StatusBar } from 'expo-status-bar'
import React, { useEffect, useState } from 'react'
import {
    ActivityIndicator,
    Button,
    StyleSheet,
    Text,
    TextInput,
    View,
} from 'react-native'
import { Provider, useSelector } from 'react-redux'
import { APIClient } from './src/api/apiClient' // Adjust path to your SimpleApi implementation
import { AuthFacade } from './src/store/auth/facade'
import { authSelectors } from './src/store/auth/selectors'
import { store } from './src/store/store'
import { UserFacade } from './src/store/user/facade'
import { getStoredToken } from './src/utils/jwt-utils'

function LoginScreen() {
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')

    const isLoading = useSelector(authSelectors.selectLoading)
    const error = useSelector(authSelectors.selectError)
    const isAuthenticated = useSelector(authSelectors.selectIsAuthenticated)

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
            <StatusBar style="auto" />
        </View>
    )
}

export default function App() {
    useEffect(() => {
        APIClient.setup();
    }, []);

    return (
        <Provider store={store}>
            <LoginScreen />
        </Provider>
    );
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
