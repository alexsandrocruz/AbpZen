import { useForm } from "react-hook-form";
  import { zodResolver } from "@hookform/resolvers/zod";
  import * as z from "zod";
  import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
  } from "@/components/ui/dialog";
  import { Button } from "@/components/ui/button";
  import { Input } from "@/components/ui/input";
  import { Label } from "@/components/ui/label";
  import { Checkbox } from "@/components/ui/checkbox";
  import { Loader2 } from "lucide-react";
  import { useEffect } from "react";
  import { useCreateUsuUsuarios, useUpdateUsuUsuarios } from "@/lib/abp/hooks/useUsuUsuarioses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idUsuario: z.any(),
  
    nome: z.any(),
  
    sobrenome: z.any(),
  
    idArea: z.any(),
  
    idCargo: z.any(),
  
    login: z.any(),
  
    senha: z.any(),
  
    diaNascimento: z.any(),
  
    mesNascimento: z.any(),
  
    anoNascimento: z.any(),
  
    email: z.any(),
  
    telCelular: z.any(),
  
    telFixo: z.any(),
  
    endereco: z.any(),
  
    numero: z.any(),
  
    complemento: z.any(),
  
    bairro: z.any(),
  
    cep: z.any(),
  
    estado: z.any(),
  
    cidade: z.any(),
  
    cpf: z.any(),
  
    banco: z.any(),
  
    agencia: z.any(),
  
    conta: z.any(),
  
    foto: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    cor: z.any(),
  
    dashboardInicial: z.any(),
  
    tokenPhoneApp: z.any(),
  
    estadoCivil: z.any(),
  
    nrFilhos: z.any(),
  
    idadeFilhoMenor: z.any(),
  
    formacaoAcademica: z.any(),
  
    regiao: z.any(),
  
    idSuperior: z.any(),
  
    master: z.any(),
  
    mediaConsumoLitro: z.any(),
  
    distanciasIguais: z.any(),
  
    chaveChamados: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface UsuUsuariosFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function UsuUsuariosForm({
  isOpen,
  onClose,
  initialValues,
}: UsuUsuariosFormProps) {
  const isEditing = !!initialValues;

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
    watch,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: initialValues || {},
  });

  useEffect(() => {
    if (initialValues) {
      reset(initialValues);
    } else {
      reset({});
    }
  }, [initialValues, reset]);

  const createMutation = useCreateUsuUsuarios();
const updateMutation = useUpdateUsuUsuarios();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("usuUsuarios updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("usuUsuarios created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save usuusuarios:", error);
    toast.error(error.message || "Failed to save usuusuarios");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit usuUsuarios": "Create usuUsuarios" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the usuusuarios." : "Fill in the details to create a new usuusuarios." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idUsuario" > idUsuario</Label>

<Input id="idUsuario" type = "number" step = "any" {...register("idUsuario") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="nome" > nome</Label>

<Input id="nome" {...register("nome") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="sobrenome" > sobrenome</Label>

<Input id="sobrenome" {...register("sobrenome") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idArea" > idArea * </Label>

<Input id="idArea" type = "number" step = "any" {...register("idArea") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCargo" > idCargo * </Label>

<Input id="idCargo" type = "number" step = "any" {...register("idCargo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="login" > login</Label>

<Input id="login" {...register("login") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="senha" > senha</Label>

<Input id="senha" {...register("senha") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="diaNascimento" > diaNascimento</Label>

<Input id="diaNascimento" type = "number" step = "any" {...register("diaNascimento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="mesNascimento" > mesNascimento</Label>

<Input id="mesNascimento" type = "number" step = "any" {...register("mesNascimento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="anoNascimento" > anoNascimento</Label>

<Input id="anoNascimento" type = "number" step = "any" {...register("anoNascimento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="email" > email</Label>

<Input id="email" {...register("email") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="telCelular" > telCelular</Label>

<Input id="telCelular" {...register("telCelular") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="telFixo" > telFixo</Label>

<Input id="telFixo" {...register("telFixo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="endereco" > endereco</Label>

<Input id="endereco" {...register("endereco") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="numero" > numero</Label>

<Input id="numero" {...register("numero") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="complemento" > complemento</Label>

<Input id="complemento" {...register("complemento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="bairro" > bairro</Label>

<Input id="bairro" {...register("bairro") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="cep" > cep</Label>

<Input id="cep" {...register("cep") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="estado" > estado</Label>

<Input id="estado" {...register("estado") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="cidade" > cidade</Label>

<Input id="cidade" {...register("cidade") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="cpf" > cpf</Label>

<Input id="cpf" {...register("cpf") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="banco" > banco</Label>

<Input id="banco" {...register("banco") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="agencia" > agencia</Label>

<Input id="agencia" {...register("agencia") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="conta" > conta</Label>

<Input id="conta" {...register("conta") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="foto" > foto</Label>

<Input id="foto" {...register("foto") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="ativo" > ativo</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="ativo"
checked = { watch("ativo") }
onCheckedChange = {(checked) => setValue("ativo", !!checked)}
              />
  < label htmlFor = "ativo" className = "text-sm font-normal" >
    { watch("ativo") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="tsInclusao" > tsInclusao</Label>

<Input id="tsInclusao" type = "date" {...register("tsInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsAlteracao" > tsAlteracao</Label>

<Input id="tsAlteracao" type = "date" {...register("tsAlteracao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="cor" > cor</Label>

<Input id="cor" {...register("cor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dashboardInicial" > dashboardInicial</Label>

<Input id="dashboardInicial" {...register("dashboardInicial") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tokenPhoneApp" > tokenPhoneApp</Label>

<Input id="tokenPhoneApp" {...register("tokenPhoneApp") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="estadoCivil" > estadoCivil</Label>

<Input id="estadoCivil" {...register("estadoCivil") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="nrFilhos" > nrFilhos</Label>

<Input id="nrFilhos" type = "number" step = "any" {...register("nrFilhos") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idadeFilhoMenor" > idadeFilhoMenor</Label>

<Input id="idadeFilhoMenor" type = "number" step = "any" {...register("idadeFilhoMenor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="formacaoAcademica" > formacaoAcademica</Label>

<Input id="formacaoAcademica" {...register("formacaoAcademica") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="regiao" > regiao</Label>

<Input id="regiao" {...register("regiao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idSuperior" > idSuperior</Label>

<Input id="idSuperior" type = "number" step = "any" {...register("idSuperior") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="master" > master</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="master"
checked = { watch("master") }
onCheckedChange = {(checked) => setValue("master", !!checked)}
              />
  < label htmlFor = "master" className = "text-sm font-normal" >
    { watch("master") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="mediaConsumoLitro" > mediaConsumoLitro</Label>

<Input id="mediaConsumoLitro" type = "number" step = "any" {...register("mediaConsumoLitro") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="distanciasIguais" > distanciasIguais</Label>

<Input id="distanciasIguais" {...register("distanciasIguais") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="chaveChamados" > chaveChamados</Label>

<Input id="chaveChamados" {...register("chaveChamados") } />

</div>


<DialogFooter>
  <Button type="button" variant = "outline" onClick = { onClose } disabled = { isSubmitting } >
    Cancel
    </Button>
    < Button type = "submit" disabled = { isSubmitting } >
      { isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
{ isEditing ? "Save Changes" : "Create" }
</Button>
  </DialogFooter>
  </form>
  </DialogContent>
  </Dialog>
  );
}
