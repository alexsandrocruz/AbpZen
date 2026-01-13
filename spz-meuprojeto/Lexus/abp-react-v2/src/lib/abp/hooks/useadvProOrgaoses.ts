import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvProOrgaosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvProOrgaoses(input: GetAdvProOrgaosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advProOrgaoses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pro-orgaos", {
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

export function useAllAdvProOrgaoses() {
  return useQuery({
    queryKey: ["advProOrgaoses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pro-orgaos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvProOrgaos(id: string) {
  return useQuery({
    queryKey: ["advProOrgaos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-pro-orgaos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvProOrgaos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-pro-orgaos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProOrgaoses"] });
    },
  });
}

export function useUpdateAdvProOrgaos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-pro-orgaos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advProOrgaoses"] });
      queryClient.invalidateQueries({ queryKey: ["advProOrgaos", data.id] });
    },
  });
}

export function useDeleteAdvProOrgaos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-pro-orgaos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProOrgaoses"] });
    },
  });
}


