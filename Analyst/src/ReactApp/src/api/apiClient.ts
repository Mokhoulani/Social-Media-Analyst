import { Observable, throwError } from 'rxjs';
import { ajax, AjaxRequest, AjaxResponse, AjaxError } from 'rxjs/ajax';
import { catchError, map } from 'rxjs/operators';
import { getStoredToken } from '../utils/jwt-utils';

interface EnhancedConfig extends Partial<AjaxRequest> {
    skipAuth?: boolean;
    retryCount?: number;
}

type RequestInterceptor = (config: EnhancedConfig) => EnhancedConfig;
type ResponseInterceptor = (response: any) => any;

export class APIClient {
    private static baseURL = process.env.API_URL || 'http://localhost:5031/api';
    private static requestInterceptors: RequestInterceptor[] = [];
    private static responseInterceptors: ResponseInterceptor[] = [];

    static setup() {
        this.addRequestInterceptor((config) => {
            if (!config.skipAuth) {
                const token = getStoredToken();
                config.headers = {
                    ...config.headers,
                    Authorization: `Bearer ${token}`,
                };
            }
            return config;
        });
    }

    static addRequestInterceptor(interceptor: RequestInterceptor) {
        this.requestInterceptors.push(interceptor);
    }

    static addResponseInterceptor(interceptor: ResponseInterceptor) {
        this.responseInterceptors.push(interceptor);
    }

    static get<T>(url: string, config?: EnhancedConfig): Observable<T> {
        return this.request<T>({ ...config, method: 'GET', url });
    }

    static post<T, D = any>(url: string, body?: D, config?: EnhancedConfig): Observable<T> {
        return this.request<T>({ ...config, method: 'POST', url, body });
    }

    static put<T, D = any>(url: string, body?: D, config?: EnhancedConfig): Observable<T> {
        return this.request<T>({ ...config, method: 'PUT', url, body });
    }

    static patch<T, D = any>(url: string, body?: D, config?: EnhancedConfig): Observable<T> {
        return this.request<T>({ ...config, method: 'PATCH', url, body });
    }

    static delete<T>(url: string, config?: EnhancedConfig): Observable<T> {
        return this.request<T>({ ...config, method: 'DELETE', url });
    }

    private static applyInterceptors(config: EnhancedConfig): EnhancedConfig {
        return this.requestInterceptors.reduce((acc, interceptor) => interceptor(acc), config);
    }

    private static applyResponseInterceptors(response: any): any {
        return this.responseInterceptors.reduce((acc, interceptor) => interceptor(acc), response);
    }

    private static request<T>(config: EnhancedConfig): Observable<T> {
        const finalConfig = this.applyInterceptors(config);
        const url = `${this.baseURL}${finalConfig.url}`;

        return ajax<T>({
            ...finalConfig,
            url,
        }).pipe(
            map((res: AjaxResponse<T>) => {
                const intercepted = this.applyResponseInterceptors(res.response);
                return intercepted;
            }),
            catchError((error: AjaxError) => throwError(() => this.handleError(error))),
        );
    }

    private static handleError(error: AjaxError) {
        if (!error.response) {
            return {
                status: 0,
                message: 'Network Error',
                details: error.message || 'Cannot connect to server',
            };
        }

        return {
            status: error.status,
            message: error.response?.message ?? 'Error',
            details: error.response,
        };
    }
}
