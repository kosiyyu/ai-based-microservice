import { ref } from 'vue';
import { jwtDecode } from 'jwt-decode';

type PartialCustomJwtPayload = {
    'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name': string;
}

export const isAuthenticated = ref(false);

export function getName(jwt: string): string {
    return jwtDecode<PartialCustomJwtPayload>(jwt)['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
}