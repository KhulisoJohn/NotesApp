// jest.config.js
module.exports = {
  preset: 'ts-jest',
testEnvironment: 'jest-environment-jsdom',
setupFilesAfterEnv: ['<rootDir>/jest.setup.ts'],
  transform: {
    '^.+\\.(ts|tsx)$': 'ts-jest',
  },
  moduleFileExtensions: ['ts', 'tsx', 'js', 'jsx', 'json', 'node'],
};
