import { LoginScreen } from '../screens/LoginScreen'
import { SignUpScreen } from '../screens/SignUpScreen'
import { createGenericTopTabsNavigator } from '../shared/components/navigation/GenericTopTabsNavigator'

const AuthTopTabsNavigator = createGenericTopTabsNavigator<
    'signIn' | 'signUp'
>()

export default function AuthTabs() {
    return (
        <AuthTopTabsNavigator
            routes={[
                { name: 'signUp', component: SignUpScreen },
                { name: 'signIn', component: LoginScreen },
            ]}
        />
    )
}
