import { useState, useEffect } from "react";
import { useAuth } from "@/lib/auth-store";
import { useLocation } from "wouter";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Command, Loader2 } from "lucide-react";
import { TactileCard } from "@/components/ui/tactile-card";
import { useToast } from "@/hooks/use-toast";

export default function AuthPage() {
  const { user, currentWorkspace, checkAuth } = useAuth();
  const [, setLocation] = useLocation();

  useEffect(() => {
    checkAuth();
  }, []);

  useEffect(() => {
    if (user) {
      if (user.globalRole === 'HOST' && !currentWorkspace) {
        setLocation('/host/dashboard');
      } else if (currentWorkspace) {
        setLocation(`/${currentWorkspace.slug}/dashboard`);
      }
    }
  }, [user, currentWorkspace, setLocation]);

  const [mode, setMode] = useState<'login' | 'register'>('login');
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [name, setName] = useState("");
  const [workspaceName, setWorkspaceName] = useState("");
  const [workspaceSlug, setWorkspaceSlug] = useState("");
  
  const { login, register, isLoading, error } = useAuth();
  const { toast } = useToast();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    try {
      if (mode === 'login') {
        await login(email, password);
        const { currentWorkspace, user: authUser } = useAuth.getState();
        if (authUser?.globalRole === 'HOST' && !currentWorkspace) {
          setLocation('/host/dashboard');
        } else {
          setLocation(`/${currentWorkspace?.slug || 'workspace'}/dashboard`);
        }
        toast({ title: "Bem-vindo de volta!", description: "Login realizado com sucesso." });
      } else {
        await register({ email, password, name, workspaceName, workspaceSlug });
        const workspace = useAuth.getState().currentWorkspace;
        setLocation(`/${workspace?.slug}/dashboard`);
        toast({ title: "Bem-vindo ao Dominus!", description: "Seu workspace está pronto." });
      }
    } catch (err: any) {
      toast({ 
        title: "Erro", 
        description: err.message || "Algo deu errado",
        variant: "destructive" 
      });
    }
  };

  return (
    <div className="min-h-screen w-full flex items-center justify-center bg-muted/30 p-4">
      <div className="w-full max-w-md space-y-6">
        <div className="flex flex-col items-center space-y-2 text-center">
          <div className="size-12 rounded-xl bg-primary text-primary-foreground flex items-center justify-center shadow-xl shadow-primary/20">
            <Command className="size-7" />
          </div>
          <h1 className="text-3xl font-display font-bold tracking-tight">
            {mode === 'login' ? 'Bem-vindo de volta' : 'Comece agora'}
          </h1>
          <p className="text-muted-foreground">
            {mode === 'login' ? 'Entre no seu workspace Dominus' : 'Crie seu workspace'}
          </p>
        </div>

        <TactileCard className="p-6 md:p-8 bg-card/50 backdrop-blur-sm">
          <form onSubmit={handleSubmit} className="space-y-4">
            {mode === 'register' && (
              <>
                <div className="space-y-2">
                  <Label htmlFor="name">Nome Completo</Label>
                  <Input 
                    id="name" 
                    type="text" 
                    placeholder="João Silva" 
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                    required
                    className="h-11 bg-background"
                    data-testid="input-name"
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="workspaceName">Nome do Workspace</Label>
                  <Input 
                    id="workspaceName" 
                    type="text" 
                    placeholder="Minha Empresa" 
                    value={workspaceName}
                    onChange={(e) => {
                      setWorkspaceName(e.target.value);
                      setWorkspaceSlug(e.target.value.toLowerCase().replace(/\s+/g, '-').replace(/[^\w-]/g, ''));
                    }}
                    required
                    className="h-11 bg-background"
                    data-testid="input-workspace-name"
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="workspaceSlug">URL do Workspace</Label>
                  <div className="flex items-center gap-2">
                    <span className="text-sm text-muted-foreground">dominus.app/</span>
                    <Input 
                      id="workspaceSlug" 
                      type="text" 
                      placeholder="minha-empresa" 
                      value={workspaceSlug}
                      onChange={(e) => setWorkspaceSlug(e.target.value.toLowerCase().replace(/\s+/g, '-').replace(/[^\w-]/g, ''))}
                      required
                      className="h-11 bg-background flex-1"
                      data-testid="input-workspace-slug"
                    />
                  </div>
                </div>
              </>
            )}

            <div className="space-y-2">
              <Label htmlFor="email">E-mail</Label>
              <Input 
                id="email" 
                type="email" 
                placeholder="nome@empresa.com" 
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
                className="h-11 bg-background"
                data-testid="input-email"
              />
            </div>
            <div className="space-y-2">
              <div className="flex items-center justify-between">
                <Label htmlFor="password">Senha</Label>
                {mode === 'login' && (
                  <a href="/forgot-password" className="text-xs font-medium text-primary hover:underline" data-testid="link-forgot-password">Esqueceu?</a>
                )}
              </div>
              <Input 
                id="password" 
                type="password" 
                placeholder="••••••••"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
                className="h-11 bg-background"
                data-testid="input-password"
              />
            </div>

            <Button 
              type="submit" 
              className="w-full h-11 text-base shadow-lg shadow-primary/20" 
              disabled={isLoading}
              data-testid="button-submit"
            >
              {isLoading && <Loader2 className="mr-2 size-4 animate-spin" />}
              {mode === 'login' ? 'Entrar' : 'Criar Workspace'}
            </Button>
          </form>

          <div className="mt-6 text-center text-sm text-muted-foreground">
            {mode === 'login' ? (
              <>
                Não tem uma conta?{' '}
                <button 
                  onClick={() => setMode('register')} 
                  className="font-medium text-primary hover:underline"
                  data-testid="button-switch-register"
                >
                  Criar Workspace
                </button>
              </>
            ) : (
              <>
                Já tem uma conta?{' '}
                <button 
                  onClick={() => setMode('login')} 
                  className="font-medium text-primary hover:underline"
                  data-testid="button-switch-login"
                >
                  Entrar
                </button>
              </>
            )}
          </div>
        </TactileCard>
      </div>
    </div>
  );
}
