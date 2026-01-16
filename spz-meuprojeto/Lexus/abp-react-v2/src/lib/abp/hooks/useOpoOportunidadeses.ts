import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetOpoOportunidadesesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  opoOrcamentosId?: string;
  flwFollowsId?: string;
  }

export function useOpoOportunidadeses(input: GetOpoOportunidadesesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["opoOportunidadeses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/opo-oportunidades", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllOpoOportunidadeses() {
  return useQuery({
    queryKey: ["opoOportunidadeses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/opo-oportunidades", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useOpoOportunidades(id: string) {
  return useQuery({
    queryKey: ["opoOportunidades", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/opo-oportunidades/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateOpoOportunidades() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/opo-oportunidades", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["opoOportunidadeses"] });
    },
  });
}

export function useUpdateOpoOportunidades() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/opo-oportunidades/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["opoOportunidadeses"] });
      queryClient.invalidateQueries({ queryKey: ["opoOportunidades", data.id] });
    },
  });
}

export function useDeleteOpoOportunidades() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/opo-oportunidades/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["opoOportunidadeses"] });
    },
  });
}





/**
 * Toggle hook for many-to-many relationship: opoOrcamentos <-> flwFollows
 * Given a opoOrcamentos, toggle a flwFollows
 */
export function useToggleFlwFollows(opoOrcamentosId: string) {
  const queryClient = useQueryClient();
  const createMutation = useCreateOpoOportunidades();
  const deleteMutation = useDeleteOpoOportunidades();
  const { data: existing } = useOpoOportunidadeses({ opoOrcamentosId: opoOrcamentosId, maxResultCount: 1000 });

  return useMutation({
    mutationFn: async ({ flwFollowsId, isChecked }: { flwFollowsId: string; isChecked: boolean }) => {
      if (isChecked) {
        // Remove relationship
        const record = existing?.items?.find((i: any) => i.flwFollowsId === flwFollowsId);
        if (record) {
          await deleteMutation.mutateAsync(record.id);
        }
      } else {
        // Add relationship
        await createMutation.mutateAsync({
          opoOrcamentosId: opoOrcamentosId,
          flwFollowsId: flwFollowsId,
        });
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["opoOportunidadeses"] });
    },
  });
}

/**
 * Toggle hook for many-to-many relationship: opoOrcamentos <-> flwFollows
 * Given a flwFollows, toggle a opoOrcamentos
 */
export function useToggleOpoOrcamentos(flwFollowsId: string) {
  const queryClient = useQueryClient();
  const createMutation = useCreateOpoOportunidades();
  const deleteMutation = useDeleteOpoOportunidades();
  const { data: existing } = useOpoOportunidadeses({ flwFollowsId: flwFollowsId, maxResultCount: 1000 });

  return useMutation({
    mutationFn: async ({ opoOrcamentosId, isChecked }: { opoOrcamentosId: string; isChecked: boolean }) => {
      if (isChecked) {
        // Remove relationship
        const record = existing?.items?.find((i: any) => i.opoOrcamentosId === opoOrcamentosId);
        if (record) {
          await deleteMutation.mutateAsync(record.id);
        }
      } else {
        // Add relationship
        await createMutation.mutateAsync({
          opoOrcamentosId: opoOrcamentosId,
          flwFollowsId: flwFollowsId,
        });
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["opoOportunidadeses"] });
    },
  });
}

