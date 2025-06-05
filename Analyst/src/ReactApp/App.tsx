import { Provider } from 'react-redux'
import Main from './src/main/Main'
import { store } from './src/store/store' // Redux store

export default function App() {
    return (
        <Provider store={store}>
            <Main />
        </Provider>
    )
}
