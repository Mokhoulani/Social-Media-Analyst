import { HomeScreen } from '../screens/HomeScreen'
import { LoginScreen } from '../screens/LoginScreen'
import { createGenericTopTabsNavigator } from '../shared/components/navigation/GenericTopTabsNavigator'

const AuthTopTabsNavigator = createGenericTopTabsNavigator<
    'signIn' | 'signUp'
>()

export default function AuthTabs() {
    return (
        <AuthTopTabsNavigator
            routes={[
                { name: 'signIn', component: LoginScreen },
                { name: 'signUp', component: HomeScreen },
            ]}
        />
    )
}
