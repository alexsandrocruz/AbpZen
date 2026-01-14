import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Command, Loader2, ArrowLeft, Mail, CheckCircle } from "lucide-react";
import { TactileCard } from "@/components/ui/tactile-card";
import { useToast } from "@/hooks/use-toast";
import { Link } from "wouter";

export default function ForgotPasswordPage() {
  const [email, setEmail] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [isSuccess, setIsSuccess] = useState(false);
  const { toast } = useToast();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!email) {
      toast({ 
        title: "Erro", 
        description: "Digite seu email",
        variant: "destructive" 
      });
      return;
    }

    setIsLoading(true);
    
    try {
      const res = await fetch("/api/auth/forgot-password", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email }),
      });

      const data = await res.json();
      
      setIsSuccess(true);
      toast({ 
        title: "Email enviado!", 
        description: data.message 
      });
    } catch (err: any) {
      toast({ 
        title: "Erro", 
        description: "Algo deu errado. Tente novamente.",
        variant: "destructive" 
      });
    } finally {
      setIsLoading(false);
    }
  };

  if (isSuccess) {
    return (
      <div className="min-h-screen w-full flex items-center justify-center bg-muted/30 p-4">
        <div className="w-full max-w-md space-y-6">
          <div className="flex flex-col items-center space-y-2 text-center">
            <div className="size-12 rounded-xl bg-green-500 text-white flex items-center justify-center shadow-xl shadow-green-500/20">
              <CheckCircle className="size-7" />
            </div>
            <h1 className="text-3xl font-display font-bold tracking-tight">
              Verifique seu email
            </h1>
            <p className="text-muted-foreground">
              Se o email estiver cadastrado, você receberá as instruções para redefinir sua senha.
            </p>
          </div>

          <TactileCard className="p-6 md:p-8 bg-card/50 backdrop-blur-sm">
            <div className="space-y-4">
              <div className="flex items-center justify-center gap-3 p-4 bg-muted/50 rounded-lg">
                <Mail className="size-5 text-primary" />
                <span className="text-sm font-medium">{email}</span>
              </div>
              
              <p className="text-sm text-muted-foreground text-center">
                Não recebeu o email? Verifique sua pasta de spam ou tente novamente em alguns minutos.
              </p>

              <div className="flex flex-col gap-2">
                <Button 
                  variant="outline"
                  onClick={() => setIsSuccess(false)}
                  className="w-full"
                  data-testid="button-try-again"
                >
                  Tentar outro email
                </Button>
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

  return (
    <div className="min-h-screen w-full flex items-center justify-center bg-muted/30 p-4">
      <div className="w-full max-w-md space-y-6">
        <div className="flex flex-col items-center space-y-2 text-center">
          <div className="size-12 rounded-xl bg-primary text-primary-foreground flex items-center justify-center shadow-xl shadow-primary/20">
            <Command className="size-7" />
          </div>
          <h1 className="text-3xl font-display font-bold tracking-tight">
            Esqueceu sua senha?
          </h1>
          <p className="text-muted-foreground">
            Digite seu email e enviaremos instruções para redefinir sua senha.
          </p>
        </div>

        <TactileCard className="p-6 md:p-8 bg-card/50 backdrop-blur-sm">
          <form onSubmit={handleSubmit} className="space-y-4">
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

            <Button 
              type="submit" 
              className="w-full h-11 text-base shadow-lg shadow-primary/20" 
              disabled={isLoading}
              data-testid="button-submit"
            >
              {isLoading && <Loader2 className="mr-2 size-4 animate-spin" />}
              Enviar instruções
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
