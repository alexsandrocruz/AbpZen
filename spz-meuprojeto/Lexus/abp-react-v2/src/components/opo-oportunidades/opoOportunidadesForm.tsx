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
  import { useCreateOpoOportunidades, useUpdateOpoOportunidades } from "@/lib/abp/hooks/useOpoOportunidadeses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idOportunidade: z.any(),
  
    idCliente: z.any(),
  
    idUsuario: z.any(),
  
    idTipo: z.any(),
  
    idSituacao: z.any(),
  
    titulo: z.any(),
  
    numero: z.any(),
  
    dataInicio: z.any(),
  
    dataEstimada: z.any(),
  
    valorEstimado: z.any(),
  
    comentario: z.any(),
  
    aproveitada: z.any(),
  
    aproveitadaData: z.any(),
  
    cancelada: z.any(),
  
    canceladaMotivo: z.any(),
  
    canceladaData: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    indicadorCanceladoVisto: z.any(),
  
    valorEstimadoMensal: z.any(),
  
    deProcesso: z.any(),
  
    aproveitadaMotivo: z.any(),
  
    numeroProcesso: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface OpoOportunidadesFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function OpoOportunidadesForm({
  isOpen,
  onClose,
  initialValues,
}: OpoOportunidadesFormProps) {
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

  const createMutation = useCreateOpoOportunidades();
const updateMutation = useUpdateOpoOportunidades();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("opoOportunidades updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("opoOportunidades created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save opooportunidades:", error);
    toast.error(error.message || "Failed to save opooportunidades");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit opoOportunidades": "Create opoOportunidades" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the opooportunidades." : "Fill in the details to create a new opooportunidades." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idOportunidade" > idOportunidade</Label>

<Input id="idOportunidade" type = "number" step = "any" {...register("idOportunidade") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCliente" > idCliente * </Label>

<Input id="idCliente" type = "number" step = "any" {...register("idCliente") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idUsuario" > idUsuario * </Label>

<Input id="idUsuario" type = "number" step = "any" {...register("idUsuario") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idTipo" > idTipo * </Label>

<Input id="idTipo" type = "number" step = "any" {...register("idTipo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idSituacao" > idSituacao * </Label>

<Input id="idSituacao" type = "number" step = "any" {...register("idSituacao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="titulo" > titulo</Label>

<Input id="titulo" {...register("titulo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="numero" > numero</Label>

<Input id="numero" {...register("numero") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataInicio" > dataInicio</Label>

<Input id="dataInicio" {...register("dataInicio") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataEstimada" > dataEstimada</Label>

<Input id="dataEstimada" {...register("dataEstimada") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="valorEstimado" > valorEstimado</Label>

<Input id="valorEstimado" type = "number" step = "any" {...register("valorEstimado") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="comentario" > comentario</Label>

<Input id="comentario" {...register("comentario") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="aproveitada" > aproveitada</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="aproveitada"
checked = { watch("aproveitada") }
onCheckedChange = {(checked) => setValue("aproveitada", !!checked)}
              />
  < label htmlFor = "aproveitada" className = "text-sm font-normal" >
    { watch("aproveitada") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="aproveitadaData" > aproveitadaData</Label>

<Input id="aproveitadaData" {...register("aproveitadaData") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="cancelada" > cancelada</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="cancelada"
checked = { watch("cancelada") }
onCheckedChange = {(checked) => setValue("cancelada", !!checked)}
              />
  < label htmlFor = "cancelada" className = "text-sm font-normal" >
    { watch("cancelada") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="canceladaMotivo" > canceladaMotivo</Label>

<Input id="canceladaMotivo" {...register("canceladaMotivo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="canceladaData" > canceladaData</Label>

<Input id="canceladaData" {...register("canceladaData") } />

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
  <Label htmlFor="indicadorCanceladoVisto" > indicadorCanceladoVisto</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="indicadorCanceladoVisto"
checked = { watch("indicadorCanceladoVisto") }
onCheckedChange = {(checked) => setValue("indicadorCanceladoVisto", !!checked)}
              />
  < label htmlFor = "indicadorCanceladoVisto" className = "text-sm font-normal" >
    { watch("indicadorCanceladoVisto") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="valorEstimadoMensal" > valorEstimadoMensal</Label>

<Input id="valorEstimadoMensal" type = "number" step = "any" {...register("valorEstimadoMensal") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="deProcesso" > deProcesso</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="deProcesso"
checked = { watch("deProcesso") }
onCheckedChange = {(checked) => setValue("deProcesso", !!checked)}
              />
  < label htmlFor = "deProcesso" className = "text-sm font-normal" >
    { watch("deProcesso") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="aproveitadaMotivo" > aproveitadaMotivo</Label>

<Input id="aproveitadaMotivo" {...register("aproveitadaMotivo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="numeroProcesso" > numeroProcesso</Label>

<Input id="numeroProcesso" type = "number" step = "any" {...register("numeroProcesso") } />

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
