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
  import { useCreateAdvPreMetas, useUpdateAdvPreMetas } from "@/lib/abp/hooks/useAdvPreMetases";
import { toast } from "sonner";

const formSchema = z.object({
  
    idMeta: z.any(),
  
    tipo: z.any(),
  
    idResponsavel: z.any(),
  
    idEscritorio: z.any(),
  
    qtde: z.any(),
  
    tsInclusao: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvPreMetasFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvPreMetasForm({
  isOpen,
  onClose,
  initialValues,
}: AdvPreMetasFormProps) {
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

  const createMutation = useCreateAdvPreMetas();
const updateMutation = useUpdateAdvPreMetas();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advPreMetas updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advPreMetas created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advpremetas:", error);
    toast.error(error.message || "Failed to save advpremetas");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advPreMetas": "Create advPreMetas" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advpremetas." : "Fill in the details to create a new advpremetas." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idMeta" > idMeta</Label>

<Input id="idMeta" type = "number" step = "any" {...register("idMeta") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tipo" > tipo</Label>

<Input id="tipo" {...register("tipo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idResponsavel" > idResponsavel</Label>

<Input id="idResponsavel" type = "number" step = "any" {...register("idResponsavel") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idEscritorio" > idEscritorio</Label>

<Input id="idEscritorio" type = "number" step = "any" {...register("idEscritorio") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="qtde" > qtde</Label>

<Input id="qtde" type = "number" step = "any" {...register("qtde") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsInclusao" > tsInclusao</Label>

<Input id="tsInclusao" type = "date" {...register("tsInclusao") } />

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
