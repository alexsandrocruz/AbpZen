# ZenCode Generator Designer

Ferramenta visual para modelagem de entidades e geração de código para ABP Pro.

## Como Rodar (Dia a Dia)

Para rodar o Designer e o Backend Bridge (necessário para salvar arquivos no seu projeto local) simultaneamente, use o script de conveniência:

```bash
./run.sh
```

Ou via NPM:

```bash
npm start
```

### URLs Locais
- **Designer (UI)**: `http://localhost:5173`
- **Bridge (Backend)**: `http://localhost:3001`

## Funcionalidades
- Modelagem Visual de Entidades (React Flow)
- Geração de 26+ arquivos por entidade (ABP Pro standard)
- Escrita direta no sistema de arquivos
- Injeção inteligente de código em arquivos existentes
