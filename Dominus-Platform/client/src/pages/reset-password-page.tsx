import { useState, useEffect } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Command, Loader2, ArrowLeft, CheckCircle, AlertCircle } from "lucide-react";
import { TactileCard } from "@/components/ui/tactile-card";
import { useToast } from "@/hooks/use-toast";
import { Link, useParams, useLocation } from "wouter";

export default function ResetPasswordPage() {
  const params = useParams<{ token: string }>();
  const [, setLocation] = useLocation();
  const token = params.token || "";
  
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [isValidating, setIsValidating] = useState(true);
  const [isTokenValid, setIsTokenValid] = useState(false);
  const [isSuccess, setIsSuccess] = useState(false);
  const [tokenError, setTokenError] = useState("");
  const { toast } = useToast();

  useEffect(() => {
    const validateToken = async () => {
      if (!token) {
        setTokenError("Token não fornecido");
        setIsValidating(false);
        return;
      }

      try {
        const res = await fetch(`/api/auth/validate-reset-token/${token}`);
        const data = await res.json();
        
        if (data.valid) {
          setIsTokenValid(true);
        } else {
          setTokenError(data.message || "Token inválido ou expirado");
        }
      } catch (err) {
        setTokenError("Erro ao validar token");
      } finally {
        setIsValidating(false);
      }
    };

    validateToken();
  }, [token]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (password.length < 6) {
      toast({ 
        title: "Erro", 
        description: "A senha deve ter pelo menos 6 caracteres",
        variant: "destructive" 
      });
      return;
    }

    if (password !== confirmPassword) {
      toast({ 
        title: "Erro", 
        description: "As senhas não coincidem",
        variant: "destructive" 
      });
      return;
    }

    setIsLoading(true);
    
    try {
      const res = await fetch("/api/auth/reset-password", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ token, password }),
      });

      const data = await res.json();
      
      if (!res.ok) {
        throw new Error(data.message || "Erro ao redefinir senha");
      }

      setIsSuccess(true);
      toast({ 
        title: "Senha redefinida!", 
        description: "Você já pode fazer login com sua nova senha." 
      });
    } catch (err: any) {
      toast({ 
        title: "Erro", 
        description: err.message || "Algo deu errado",
        variant: "destructive" 
      });
    } finally {
      setIsLoading(false);
    }
  };

  if (isValidating) {
    return (
      <div className="min-h-screen w-full flex items-center justify-center bg-muted/30 p-4">
        <div className="w-full max-w-md space-y-6">
          <div className="flex flex-col items-center space-y-4">
            <Loader2 className="size-8 animate-spin text-primary" />
            <p className="text-muted-foreground">Validando link...</p>
          </div>
        </div>
      </div>
    );
  }

  if (!isTokenValid && !isSuccess) {
    return (
      <div className="min-h-screen w-full flex items-center justify-center bg-muted/30 p-4">
        <div className="w-full max-w-md space-y-6">
          <div className="flex flex-col items-center space-y-2 text-center">
            <div className="size-12 rounded-xl bg-destructive text-destructive-foreground flex items-center justify-center shadow-xl">
              <AlertCircle className="size-7" />
            </div>
            <h1 className="text-3xl font-display font-bold tracking-tight">
              Link inválido
            </h1>
            <p className="text-muted-foreground">
              {tokenError}
            </p>
          </div>

          <TactileCard className="p-6 md:p-8 bg-card/50 backdrop-blur-sm">
            <div className="space-y-4">
              <p className="text-sm text-muted-foreground text-center">
                O link pode ter expirado ou já foi utilizado. Solicite um novo link de recuperação de senha.
              </p>

              <div className="flex flex-col gap-2">
                <Link href="/forgot-password">
                  <Button className="w-full" data-testid="button-request-new">
                    Solicitar novo link
                  </Button>
                </Link>
                <Link href="/login">
                  <Button variant="ghost" className="w-full" data-testid="button-back-to-login">
                    <ArrowLeft className="size-4 mr-2" />
                    Voltar para login
                  </Button>
                </Link>
              </div>
            </div>
          </TactileCard>
        </div>
      </div>
    );
  }

  if (isSuccess) {
    return (
      <div className="min-h-screen w-full flex items-center justify-center bg-muted/30 p-4">
        <div className="w-full max-w-md space-y-6">
          <div className="flex flex-col items-center space-y-2 text-center">
            <div className="size-12 rounded-xl bg-green-500 text-white flex items-center justify-center shadow-xl shadow-green-500/20">
              <CheckCircle className="size-7" />
            </div>
            <h1 className="text-3xl font-display font-bold tracking-tight">
              Senha redefinida!
            </h1>
            <p className="text-muted-foreground">
              Sua senha foi atualizada com sucesso.
            </p>
          </div>

          <TactileCard className="p-6 md:p-8 bg-card/50 backdrop-blur-sm">
            <Link href="/login">
              <Button className="w-full h-11 text-base shadow-lg shadow-primary/20" data-testid="button-go-to-login">
                Fazer login
              </Button>
            </Link>
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
            Nova senha
          </h1>
          <p className="text-muted-foreground">
            Digite sua nova senha abaixo.
          </p>
        </div>

        <TactileCard className="p-6 md:p-8 bg-card/50 backdrop-blur-sm">
          <form onSubmit={handleSubmit} className="space-y-4">
            <div className="space-y-2">
              <Label htmlFor="password">Nova senha</Label>
              <Input 
                id="password" 
                type="password" 
                placeholder="••••••••"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
                minLength={6}
                className="h-11 bg-background"
                data-testid="input-password"
              />
              <p className="text-xs text-muted-foreground">Mínimo de 6 caracteres</p>
            </div>

            <div className="space-y-2">
              <Label htmlFor="confirmPassword">Confirmar senha</Label>
              <Input 
                id="confirmPassword" 
                type="password" 
                placeholder="••••••••"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                required
                className="h-11 bg-background"
                data-testid="input-confirm-password"
              />
            </div>

            <Button 
              type="submit" 
              className="w-full h-11 text-base shadow-lg shadow-primary/20" 
              disabled={isLoading}
              data-testid="button-submit"
            >
              {isLoading && <Loader2 className="mr-2 size-4 animate-spin" />}
              Redefinir senha
            </Button>
          </form>

          <div className="mt-6 text-center">
            <Link href="/login">
              <Button variant="ghost" className="text-sm text-muted-foreground hover:text-foreground" data-testid="button-back-to-login">
                <ArrowLeft className="size-4 mr-2" />
                Voltar para login
              </Button>
            </Link>
          </div>
        </TactileCard>
      </div>
    </div>
  );
}
