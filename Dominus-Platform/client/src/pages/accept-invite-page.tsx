import { useState, useEffect } from "react";
import { useParams, useLocation } from "wouter";
import { useAuth } from "@/lib/auth-store";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Command, Loader2, Check, AlertCircle } from "lucide-react";
import { TactileCard } from "@/components/ui/tactile-card";
import { useToast } from "@/hooks/use-toast";
import { api } from "@/lib/api";

interface InviteInfo {
  email: string;
  memberRole: string;
  status: string;
  expiresAt: string;
  workspace: {
    name: string;
    slug: string;
  } | null;
}

export default function AcceptInvitePage() {
  const { token } = useParams<{ token: string }>();
  const [, setLocation] = useLocation();
  const { user, login } = useAuth();
  const { toast } = useToast();

  const [inviteInfo, setInviteInfo] = useState<InviteInfo | null>(null);
  const [loading, setLoading] = useState(true);
  const [accepting, setAccepting] = useState(false);
  const [error, setError] = useState<string | null>(null);
  
  const [mode, setMode] = useState<'login' | 'register'>('register');
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [name, setName] = useState("");

  useEffect(() => {
    async function fetchInvite() {
      try {
        const res = await fetch(`/api/invites/${token}`);
        if (!res.ok) {
          const data = await res.json();
          throw new Error(data.message || "Convite inválido");
        }
        const data = await res.json();
        setInviteInfo(data);
        setEmail(data.email);
      } catch (err: any) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }
    fetchInvite();
  }, [token]);

  const handleAcceptInvite = async () => {
    setAccepting(true);
    try {
      const res = await fetch(`/api/invites/${token}/accept`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
      });
      if (!res.ok) {
        const data = await res.json();
        throw new Error(data.message || "Erro ao aceitar convite");
      }
      const data = await res.json();
      toast({ title: "Convite aceito!", description: `Você agora é membro de ${inviteInfo?.workspace?.name}` });
      await useAuth.getState().checkAuth();
      setLocation(`/${data.workspace.slug}/dashboard`);
    } catch (err: any) {
      toast({ title: "Erro", description: err.message, variant: "destructive" });
    } finally {
      setAccepting(false);
    }
  };

  const handleRegisterAndAccept = async (e: React.FormEvent) => {
    e.preventDefault();
    setAccepting(true);
    try {
      const registerRes = await fetch('/api/auth/register-for-invite', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password, name, inviteToken: token }),
      });
      if (!registerRes.ok) {
        const data = await registerRes.json();
        throw new Error(data.message || "Erro ao registrar");
      }
      const data = await registerRes.json();
      toast({ title: "Conta criada!", description: `Bem-vindo ao ${inviteInfo?.workspace?.name}` });
      await useAuth.getState().checkAuth();
      setLocation(`/${data.workspace.slug}/dashboard`);
    } catch (err: any) {
      toast({ title: "Erro", description: err.message, variant: "destructive" });
    } finally {
      setAccepting(false);
    }
  };

  const handleLoginAndAccept = async (e: React.FormEvent) => {
    e.preventDefault();
    setAccepting(true);
    try {
      await login(email, password);
      await handleAcceptInvite();
    } catch (err: any) {
      toast({ title: "Erro", description: err.message, variant: "destructive" });
      setAccepting(false);
    }
  };

  if (loading) {
    return (
      <div className="min-h-screen w-full flex items-center justify-center bg-muted/30">
        <Loader2 className="size-8 animate-spin text-primary" />
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen w-full flex items-center justify-center bg-muted/30 p-4">
        <TactileCard className="max-w-md p-8 text-center space-y-4">
          <AlertCircle className="size-12 text-destructive mx-auto" />
          <h1 className="text-xl font-semibold">Convite Inválido</h1>
          <p className="text-muted-foreground">{error}</p>
          <Button onClick={() => setLocation("/")} data-testid="button-go-home">
            Ir para Login
          </Button>
        </TactileCard>
      </div>
    );
  }

  const roleLabels: Record<string, string> = {
    ADMIN: "Administrador",
    GERENTE: "Gerente",
    VENDEDOR: "Vendedor",
    FINANCEIRO: "Financeiro",
    MEMBRO: "Membro",
  };

  if (user) {
    return (
      <div className="min-h-screen w-full flex items-center justify-center bg-muted/30 p-4">
        <div className="w-full max-w-md space-y-6">
          <div className="flex flex-col items-center space-y-2 text-center">
            <div className="size-12 rounded-xl bg-primary text-primary-foreground flex items-center justify-center shadow-xl shadow-primary/20">
              <Command className="size-7" />
            </div>
            <h1 className="text-3xl font-display font-bold tracking-tight">
              Convite para {inviteInfo?.workspace?.name}
            </h1>
            <p className="text-muted-foreground">
              Você foi convidado como <span className="font-medium text-foreground">{roleLabels[inviteInfo?.memberRole || 'MEMBRO']}</span>
            </p>
          </div>

          <TactileCard className="p-6 md:p-8 bg-card/50 backdrop-blur-sm space-y-6">
            <div className="flex items-center gap-3 p-4 bg-muted/50 rounded-lg">
              <Check className="size-5 text-emerald-500" />
              <div>
                <p className="font-medium">Logado como {user.name}</p>
                <p className="text-sm text-muted-foreground">{user.email}</p>
              </div>
            </div>

            <Button 
              onClick={handleAcceptInvite} 
              className="w-full h-11" 
              disabled={accepting}
              data-testid="button-accept-invite"
            >
              {accepting && <Loader2 className="mr-2 size-4 animate-spin" />}
              Aceitar Convite
            </Button>
          </TactileCard>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen w-full flex items-center justify-center bg-muted/30 p-4">
      <div className="w-full max-w-md space-y-6">
        <div className="flex flex-col items-center space-y-2 text-center">
          <div className="size-12 rounded-xl bg-primary text-primary-foreground flex items-center justify-center shadow-xl shadow-primary/20">
            <Command className="size-7" />
          </div>
          <h1 className="text-3xl font-display font-bold tracking-tight">
            Convite para {inviteInfo?.workspace?.name}
          </h1>
          <p className="text-muted-foreground">
            Você foi convidado como <span className="font-medium text-foreground">{roleLabels[inviteInfo?.memberRole || 'MEMBRO']}</span>
          </p>
        </div>

        <TactileCard className="p-6 md:p-8 bg-card/50 backdrop-blur-sm">
          <form onSubmit={mode === 'register' ? handleRegisterAndAccept : handleLoginAndAccept} className="space-y-4">
            {mode === 'register' && (
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
                disabled={mode === 'register'}
                className="h-11 bg-background"
                data-testid="input-email"
              />
              {mode === 'register' && (
                <p className="text-xs text-muted-foreground">
                  O convite foi enviado para este e-mail
                </p>
              )}
            </div>

            <div className="space-y-2">
              <Label htmlFor="password">
                {mode === 'register' ? 'Criar Senha' : 'Senha'}
              </Label>
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
              disabled={accepting}
              data-testid="button-submit"
            >
              {accepting && <Loader2 className="mr-2 size-4 animate-spin" />}
              {mode === 'register' ? 'Criar Conta e Aceitar' : 'Entrar e Aceitar'}
            </Button>
          </form>

          <div className="mt-6 text-center text-sm text-muted-foreground">
            {mode === 'register' ? (
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
            ) : (
              <>
                Não tem uma conta?{' '}
                <button 
                  onClick={() => setMode('register')} 
                  className="font-medium text-primary hover:underline"
                  data-testid="button-switch-register"
                >
                  Criar Conta
                </button>
              </>
            )}
          </div>
        </TactileCard>
      </div>
    </div>
  );
}
