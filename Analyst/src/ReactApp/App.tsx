import React, { useState } from 'react'
import { Provider } from 'react-redux'
import { store } from './src/store/store' // adjust path if needed
import { StatusBar } from 'expo-status-bar'
import {
    StyleSheet,
    Text,
    View,
    TextInput,
    Button,
    ActivityIndicator,
} from 'react-native'
import { AuthFacade } from './src/store/auth/facade' // adjust path if needed
import { useSelector } from 'react-redux'
import { authSelectors } from './src/store/auth/selectors' // adjust path if needed

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
            <StatusBar style="auto" />
        </View>
    )
}

export default function App() {
    return (
        <Provider store={store}>
            <LoginScreen />
        </Provider>
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
