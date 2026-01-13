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
  import { useCreateAdvFornecedores, useUpdateAdvFornecedores } from "@/lib/abp/hooks/useAdvFornecedoreses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idFornecedor: z.any(),
  
    apelido: z.any(),
  
    nome: z.any(),
  
    email: z.any(),
  
    telCelular: z.any(),
  
    telCelularObs: z.any(),
  
    telFixo: z.any(),
  
    telFixoObs: z.any(),
  
    endereco: z.any(),
  
    numero: z.any(),
  
    complemento: z.any(),
  
    bairro: z.any(),
  
    cep: z.any(),
  
    estado: z.any(),
  
    cidade: z.any(),
  
    observacoes: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    parceiroEmProcesso: z.any(),
  
    parceiroEmProcessoPerc: z.any(),
  
    idProfissional: z.any(),
  
    foto: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvFornecedoresFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvFornecedoresForm({
  isOpen,
  onClose,
  initialValues,
}: AdvFornecedoresFormProps) {
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

  const createMutation = useCreateAdvFornecedores();
const updateMutation = useUpdateAdvFornecedores();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advFornecedores updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advFornecedores created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advfornecedores:", error);
    toast.error(error.message || "Failed to save advfornecedores");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advFornecedores": "Create advFornecedores" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advfornecedores." : "Fill in the details to create a new advfornecedores." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idFornecedor" > idFornecedor</Label>

<Input id="idFornecedor" type = "number" step = "any" {...register("idFornecedor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="apelido" > apelido</Label>

<Input id="apelido" {...register("apelido") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="nome" > nome</Label>

<Input id="nome" {...register("nome") } />

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
  <Label htmlFor="telCelularObs" > telCelularObs</Label>

<Input id="telCelularObs" {...register("telCelularObs") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="telFixo" > telFixo</Label>

<Input id="telFixo" {...register("telFixo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="telFixoObs" > telFixoObs</Label>

<Input id="telFixoObs" {...register("telFixoObs") } />

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
  <Label htmlFor="observacoes" > observacoes</Label>

<Input id="observacoes" {...register("observacoes") } />

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
  <Label htmlFor="parceiroEmProcesso" > parceiroEmProcesso</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="parceiroEmProcesso"
checked = { watch("parceiroEmProcesso") }
onCheckedChange = {(checked) => setValue("parceiroEmProcesso", !!checked)}
              />
  < label htmlFor = "parceiroEmProcesso" className = "text-sm font-normal" >
    { watch("parceiroEmProcesso") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="parceiroEmProcessoPerc" > parceiroEmProcessoPerc</Label>

<Input id="parceiroEmProcessoPerc" type = "number" step = "any" {...register("parceiroEmProcessoPerc") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idProfissional" > idProfissional</Label>

<Input id="idProfissional" type = "number" step = "any" {...register("idProfissional") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="foto" > foto</Label>

<Input id="foto" {...register("foto") } />

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
