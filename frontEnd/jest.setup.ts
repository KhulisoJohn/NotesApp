import { TextEncoder, TextDecoder } from 'util';

// @ts-expect-error: Override typing mismatch for TextEncoder/TextDecoder polyfill
(global as never).TextEncoder = TextEncoder;
// @ts-expect-error: Override typing mismatch for TextEncoder/TextDecoder polyfill
(global as never).TextDecoder = TextDecoder;