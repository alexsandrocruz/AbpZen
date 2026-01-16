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
  import { useCreateAdvVerbas, useUpdateAdvVerbas } from "@/lib/abp/hooks/useAdvVerbases";
import { toast } from "sonner";

const formSchema = z.object({
  
    idVerba: z.any(),
  
    idTipo: z.any(),
  
    idProfissional: z.any(),
  
    idProcesso: z.any(),
  
    idLancamento: z.any(),
  
    valor: z.any(),
  
    dataDe: z.any(),
  
    dataAte: z.any(),
  
    estado: z.any(),
  
    cidade: z.any(),
  
    comprovante: z.any(),
  
    comprovanteArquivo: z.any(),
  
    solicitacao: z.any(),
  
    aceito: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    incluidoPor: z.any(),
  
    alteradoPor: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvVerbasFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvVerbasForm({
  isOpen,
  onClose,
  initialValues,
}: AdvVerbasFormProps) {
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

  const createMutation = useCreateAdvVerbas();
const updateMutation = useUpdateAdvVerbas();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advVerbas updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advVerbas created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advverbas:", error);
    toast.error(error.message || "Failed to save advverbas");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advVerbas": "Create advVerbas" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advverbas." : "Fill in the details to create a new advverbas." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idVerba" > idVerba</Label>

<Input id="idVerba" type = "number" step = "any" {...register("idVerba") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idTipo" > idTipo * </Label>

<Input id="idTipo" type = "number" step = "any" {...register("idTipo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idProfissional" > idProfissional * </Label>

<Input id="idProfissional" type = "number" step = "any" {...register("idProfissional") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idProcesso" > idProcesso</Label>

<Input id="idProcesso" type = "number" step = "any" {...register("idProcesso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idLancamento" > idLancamento</Label>

<Input id="idLancamento" type = "number" step = "any" {...register("idLancamento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="valor" > valor</Label>

<Input id="valor" type = "number" step = "any" {...register("valor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataDe" > dataDe * </Label>

<Input id="dataDe" {...register("dataDe") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataAte" > dataAte * </Label>

<Input id="dataAte" {...register("dataAte") } />

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
  <Label htmlFor="comprovante" > comprovante</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="comprovante"
checked = { watch("comprovante") }
onCheckedChange = {(checked) => setValue("comprovante", !!checked)}
              />
  < label htmlFor = "comprovante" className = "text-sm font-normal" >
    { watch("comprovante") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="comprovanteArquivo" > comprovanteArquivo</Label>

<Input id="comprovanteArquivo" {...register("comprovanteArquivo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="solicitacao" > solicitacao</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="solicitacao"
checked = { watch("solicitacao") }
onCheckedChange = {(checked) => setValue("solicitacao", !!checked)}
              />
  < label htmlFor = "solicitacao" className = "text-sm font-normal" >
    { watch("solicitacao") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="aceito" > aceito</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="aceito"
checked = { watch("aceito") }
onCheckedChange = {(checked) => setValue("aceito", !!checked)}
              />
  < label htmlFor = "aceito" className = "text-sm font-normal" >
    { watch("aceito") ?"Enabled": "Disabled" }
    </label>
    </div>

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
  <Label htmlFor="incluidoPor" > incluidoPor</Label>

<Input id="incluidoPor" {...register("incluidoPor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="alteradoPor" > alteradoPor</Label>

<Input id="alteradoPor" {...register("alteradoPor") } />

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
